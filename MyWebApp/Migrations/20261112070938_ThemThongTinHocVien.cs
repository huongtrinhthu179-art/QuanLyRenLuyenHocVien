using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyWebApp.Migrations
{
    /// <inheritdoc />
    public partial class ThemThongTinHocVien : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Chi bo sung cot HocViens. Bang UserProfiles da do Program.cs tao rieng.
            migrationBuilder.AddColumn<string>(
                name: "DanhGia",
                table: "HocViens",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "DiemTieuChi1",
                table: "HocViens",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DiemTieuChi2",
                table: "HocViens",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DiemTieuChi3",
                table: "HocViens",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DiemTieuChi4",
                table: "HocViens",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DiemTieuChi5",
                table: "HocViens",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "HocViens",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GioiTinh",
                table: "HocViens",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "QueQuan",
                table: "HocViens",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SoBaoDanh",
                table: "HocViens",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SoDienThoai",
                table: "HocViens",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Khong xoa UserProfiles vi bang nay khong thuoc migration nay.

            migrationBuilder.DropColumn(
                name: "DanhGia",
                table: "HocViens");

            migrationBuilder.DropColumn(
                name: "DiemTieuChi1",
                table: "HocViens");

            migrationBuilder.DropColumn(
                name: "DiemTieuChi2",
                table: "HocViens");

            migrationBuilder.DropColumn(
                name: "DiemTieuChi3",
                table: "HocViens");

            migrationBuilder.DropColumn(
                name: "DiemTieuChi4",
                table: "HocViens");

            migrationBuilder.DropColumn(
                name: "DiemTieuChi5",
                table: "HocViens");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "HocViens");

            migrationBuilder.DropColumn(
                name: "GioiTinh",
                table: "HocViens");

            migrationBuilder.DropColumn(
                name: "QueQuan",
                table: "HocViens");

            migrationBuilder.DropColumn(
                name: "SoBaoDanh",
                table: "HocViens");

            migrationBuilder.DropColumn(
                name: "SoDienThoai",
                table: "HocViens");
        }
    }
}
