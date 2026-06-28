using System.ComponentModel.DataAnnotations;

namespace TheGalacticTournament.Api.DTOs
{
    /// <summary>
    /// Data Transfer Object used to create a new species.
    /// 
    /// This DTO receives the required information from the client when
    /// a new species is registered in the Galactic Tournament.
    /// 
    /// It also includes validation rules to ensure that the submitted data
    /// follows the business requirements before reaching the service layer.
    /// </summary>
    public class CreateSpeciesDto
    {
        /// <summary>
        /// Name of the species.
        /// 
        /// Rules:
        /// - Required.
        /// - Maximum length of 100 characters.
        /// - Only letters are allowed.
        /// - Spaces, numbers and special characters are not allowed.
        /// 
        /// Example: "Saiyan"
        /// </summary>
        [Required(ErrorMessage = "Species name is required.")]
        [StringLength(100, ErrorMessage = "Species name cannot exceed 100 characters.")]
        [RegularExpression(
            @"^[A-Za-z¡…Õ”⁄·ÈÌÛ˙—Ò]+$",
            ErrorMessage = "Species name can only contain letters. Spaces, numbers and special characters are not allowed."
        )]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Power level of the species.
        /// 
        /// Rules:
        /// - Must be a positive integer.
        /// - The minimum valid value is 1.
        /// 
        /// Example: 9500
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Power level must be a positive integer.")]
        public int PowerLevel { get; set; }

        /// <summary>
        /// Special ability of the species.
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
            @"^[A-Za-z¡…Õ”⁄·ÈÌÛ˙—Ò ]+$",
            ErrorMessage = "Special ability can only contain letters and spaces. Numbers and special characters are not allowed."
        )]
        public string SpecialAbility { get; set; } = string.Empty;
    }
}