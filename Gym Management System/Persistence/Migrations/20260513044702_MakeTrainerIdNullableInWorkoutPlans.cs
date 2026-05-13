using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagementSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MakeTrainerIdNullableInWorkoutPlans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutPlans_Trainers_TrainerId",
                table: "WorkoutPlans");

            migrationBuilder.AlterColumn<string>(
                name: "TrainerId",
                table: "WorkoutPlans",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutPlans_Trainers_TrainerId",
                table: "WorkoutPlans",
                column: "TrainerId",
                principalTable: "Trainers",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutPlans_Trainers_TrainerId",
                table: "WorkoutPlans");

            migrationBuilder.AlterColumn<string>(
                name: "TrainerId",
                table: "WorkoutPlans",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutPlans_Trainers_TrainerId",
                table: "WorkoutPlans",
                column: "TrainerId",
                principalTable: "Trainers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
