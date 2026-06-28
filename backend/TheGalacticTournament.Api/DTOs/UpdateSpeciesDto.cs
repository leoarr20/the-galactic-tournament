using System.ComponentModel.DataAnnotations;

namespace TheGalacticTournament.Api.DTOs
{
    /// <summary>
    /// Data Transfer Object used to update an existing species.
    /// 
    /// This DTO receives the new information that will replace the current
    /// values of a registered species in the Galactic Tournament.
    /// 
    /// It includes validation rules to ensure that the updated data follows
    /// the same business requirements used when creating a species.
    /// </summary>
    public class UpdateSpeciesDto
    {
        /// <summary>
        /// Updated name of the species.
        /// 
        /// Rules:
        /// - Required.
        /// - Maximum length of 100 characters.
        /// - Only letters are allowed.
        /// - Spaces, numbers and special characters are not allowed.
        /// 
        /// The service layer should also validate that this name is unique
        /// and is not already used by another species.
        /// 
        /// Example: "Saiyan"
        /// </summary>
        [Required(ErrorMessage = "Species name is required.")]
        [StringLength(100, ErrorMessage = "Species name cannot exceed 100 characters.")]
        [RegularExpression(
            @"^[A-Za-zÁÉÍÓÚáéíóúÑñ]+$",
            ErrorMessage = "Species name can only contain letters. Spaces, numbers and special characters are not allowed."
        )]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Updated power level of the species.
        /// 
        /// Rules:
        /// - Must be a positive integer.
        /// - The minimum valid value is 1.
        /// 
        /// This value is used during battles to determine the winner.
        /// 
        /// Example: 9500
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Power level must be a positive integer.")]
        public int PowerLevel { get; set; }

        /// <summary>
        /// Updated special ability of the species.
        /// 
        /// Rules:
        /// - Required.
        /// - Maximum length of 100 characters.
        /// - Only letters and spaces are allowed.
        /// - Numbers and special characters are not allowed.
        /// 
        /// Example: "Super transformation"
        /// </summary>
        [Required(ErrorMessage = "Special ability is required.")]
        [StringLength(100, ErrorMessage = "Special ability cannot exceed 100 characters.")]
        [RegularExpression(
            @"^[A-Za-zÁÉÍÓÚáéíóúÑñ ]+$",
            ErrorMessage = "Special ability can only contain letters and spaces. Numbers and special characters are not allowed."
        )]
        public string SpecialAbility { get; set; } = string.Empty;
    }
}