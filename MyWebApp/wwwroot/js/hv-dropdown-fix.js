// Fix the placement of Radzen dropdown popups inside scrolling Blazor dialogs.
// No changes to Radzen.Blazor.js or unrelated pages are required.
export function install() {
  let activeTrigger = null;
  let observer;
  let pending = false;
  let disposed = false;
  const margin = 8;

  const triggers = '.hv-rz-dropdown.rz-dropdown';
  const panelSelector = '.rz-dropdown-panel';

  function visible(element) {
    if (!element || !element.isConnected) return false;
    const style = getComputedStyle(element);
    return style.display !== 'none'
      && (style.visibility !== 'hidden' || element.dataset.hvTemporarilyHidden === '1')
      && element.getClientRects().length > 0;
  }

  function currentPanel() {
    if (!activeTrigger || !activeTrigger.isConnected) return null;
    // Radzen uses an aria relationship in some versions; prefer that when present.
    const linked = [activeTrigger, ...activeTrigger.querySelectorAll('[aria-controls], [aria-owns]')];
    for (const item of linked) {
      const id = item.getAttribute('aria-controls') || item.getAttribute('aria-owns');
      if (!id) continue;
      const linkedElement = document.getElementById(id);
      const panel = linkedElement?.matches?.(panelSelector)
        ? linkedElement : linkedElement?.querySelector?.(panelSelector);
      if (visible(panel)) return panel;
    }
    const panels = Array.from(document.querySelectorAll(panelSelector)).filter(visible);
    return panels.length ? panels[panels.length - 1] : null;
  }

  function popupPositionNode(panel) {
    // Radzen may position either the panel or the popup wrapper, depending on version.
    let node = panel;
    while (node && node !== document.body) {
      const pos = getComputedStyle(node).position;
      if (pos === 'absolute' || pos === 'fixed') return node;
      node = node.parentElement;
    }
    return panel;
  }

  function setStyleIfDifferent(el, name, value) {
    if (el.style[name] !== value) el.style[name] = value;
  }

  function alignPanel() {
    const panel = currentPanel();
    if (!panel || !visible(activeTrigger)) return;
    const anchor = activeTrigger.getBoundingClientRect();
    const vv = window.visualViewport;
    const topEdge = vv?.offsetTop ?? 0;
    const bottomEdge = topEdge + (vv?.height ?? window.innerHeight);
    const leftEdge = vv?.offsetLeft ?? 0;
    const rightEdge = leftEdge + (vv?.width ?? window.innerWidth);
    if (anchor.bottom < topEdge || anchor.top > bottomEdge) {
      // If the selected field is scrolled out of view, don't leave its menu floating.
      panel.dataset.hvTemporarilyHidden = '1';
      setStyleIfDifferent(panel, 'visibility', 'hidden');
      return;
    }
    if (panel.dataset.hvTemporarilyHidden === '1') {
      delete panel.dataset.hvTemporarilyHidden;
      setStyleIfDifferent(panel, 'visibility', 'visible');
    }

    const node = popupPositionNode(panel);
    const width = Math.min(Math.max(anchor.width, 100), Math.max(100, rightEdge - leftEdge - 2 * margin));
    const left = Math.max(leftEdge + margin, Math.min(anchor.left, rightEdge - margin - width));
    const gap = 4;
    const below = Math.max(0, bottomEdge - anchor.bottom - gap - margin);
    const above = Math.max(0, anchor.top - topEdge - gap - margin);
    const naturalHeight = Math.max(50, Math.min(240, panel.scrollHeight || panel.getBoundingClientRect().height || 200));
    const openAbove = below < Math.min(naturalHeight, 170) && above > below;
    const room = openAbove ? above : below;
    const height = Math.max(40, Math.min(naturalHeight, room));
    const top = openAbove ? anchor.top - height - gap : anchor.bottom + gap;
    const safeTop = Math.max(topEdge + margin, Math.min(top, bottomEdge - height - margin));

    // Temporarily suspend observation to avoid recursively reacting to our own styles.
    observer?.disconnect();
    setStyleIfDifferent(node, 'position', 'fixed');
    setStyleIfDifferent(node, 'top', `${Math.round(safeTop)}px`);
    setStyleIfDifferent(node, 'left', `${Math.round(left)}px`);
    setStyleIfDifferent(node, 'right', 'auto');
    setStyleIfDifferent(node, 'bottom', 'auto');
    setStyleIfDifferent(node, 'transform', 'none');
    setStyleIfDifferent(node, 'width', `${Math.round(width)}px`);
    setStyleIfDifferent(node, 'zIndex', '3100');
    setStyleIfDifferent(panel, 'maxHeight', `${Math.round(height)}px`);
    const items = panel.querySelector('.rz-dropdown-items-wrapper');
    if (items) setStyleIfDifferent(items, 'maxHeight', `${Math.max(35, Math.round(height - 4))}px`);
    if (!disposed) watch();
  }

  function schedule() {
    if (pending || disposed || !activeTrigger) return;
    pending = true;
    requestAnimationFrame(() => {
      pending = false;
      if (!disposed) alignPanel();
    });
  }

  function onPointerDown(event) {
    const target = event.target;
    if (!(target instanceof Element)) return;
    const dropdown = target.closest(triggers);
    if (dropdown) { activeTrigger = dropdown; schedule(); return; }
    if (target.closest(panelSelector)) { schedule(); return; }
    activeTrigger = null;
  }

  function onFocusIn(event) {
    const target = event.target;
    if (!(target instanceof Element)) return;
    const dropdown = target.closest(triggers);
    if (dropdown) { activeTrigger = dropdown; schedule(); }
  }

  function onKeyDown(event) {
    if (event.key === 'Escape') { activeTrigger = null; return; }
    if (event.key === 'Enter' || event.key === ' ' || event.key.startsWith('Arrow')) {
      const target = event.target;
      if (target instanceof Element) {
        const dropdown = target.closest(triggers);
        if (dropdown) activeTrigger = dropdown;
      }
      schedule();
    }
  }

  function watch() {
    observer.observe(document.body, {subtree: true, childList: true,
      attributes: true, attributeFilter: ['style', 'class', 'aria-expanded']});
  }

  observer = new MutationObserver(schedule);
  document.addEventListener('pointerdown', onPointerDown, true);
  document.addEventListener('focusin', onFocusIn, true);
  document.addEventListener('keydown', onKeyDown, true);
  document.addEventListener('scroll', schedule, true);  // also catches nested modal scroll
  window.addEventListener('resize', schedule, {passive: true});
  window.visualViewport?.addEventListener('resize', schedule, {passive: true});
  window.visualViewport?.addEventListener('scroll', schedule, {passive: true});
  watch();

  return {
    dispose() {
      disposed = true;
      activeTrigger = null;
      observer.disconnect();
      document.removeEventListener('pointerdown', onPointerDown, true);
      document.removeEventListener('focusin', onFocusIn, true);
      document.removeEventListener('keydown', onKeyDown, true);
      document.removeEventListener('scroll', schedule, true);
      window.removeEventListener('resize', schedule);
      window.visualViewport?.removeEventListener('resize', schedule);
      window.visualViewport?.removeEventListener('scroll', schedule);
    }
  };
}
