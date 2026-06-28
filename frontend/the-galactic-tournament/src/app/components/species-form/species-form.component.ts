import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NgForm } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { SpeciesService } from '../../services/species.service';
import { CreateSpecies, Species, UpdateSpecies } from '../../models/species';

/**
 * Component responsible for creating and updating species.
 *
 * Responsibilities:
 * - Validate user input before sending data to the API.
 * - Restrict invalid characters in the form fields.
 * - Support create and edit modes using the same form.
 * - Notify the parent component when a species is created or updated.
 */
@Component({
  selector: 'app-species-form',
  templateUrl: './species-form.component.html',
  styleUrls: ['./species-form.component.scss']
})
export class SpeciesFormComponent {

  /**
   * Species list received from the parent component.
   *
   * It is used to validate duplicated names in the frontend before submitting
   * the request to the backend.
   */
  @Input() speciesList: Species[] = [];

  /**
   * Event emitted after a species is created successfully.
   */
  @Output() speciesCreated = new EventEmitter<Species>();

  /**
   * Event emitted after a species is updated successfully.
   */
  @Output() speciesUpdated = new EventEmitter<Species>();

  /**
   * Form model used for both create and update operations.
   */
  species: CreateSpecies = {
    name: '',
    powerLevel: 0,
    specialAbility: ''
  };

  /**
   * Text representation of the power level field.
   *
   * This is kept as string so the component can control digit input,
   * leading zeroes and maximum length before converting to number.
   */
  powerLevelInput = '';

  /**
   * Maximum amount of digits allowed in the power level input.
   */
  readonly maxPowerLevelDigits = 10;

  /**
   * Maximum value supported by the backend because PowerLevel is an int.
   */
  readonly maxPowerLevelValue = 2147483647;

  /**
   * Indicates whether the form is editing an existing species.
   */
  isEditMode = false;

  /**
   * Identifier of the species currently being edited.
   */
  editingSpeciesId: number | null = null;

  /**
   * Indicates whether a create or update request is currently running.
   */
  loading = false;

  /**
   * Translation key displayed when an error occurs.
   */
  errorMessage = '';

  /**
   * Translation key displayed when an operation completes successfully.
   */
  successMessage = '';

  /**
   * Regular expression for species names.
   *
   * Only letters are allowed. Spaces, numbers and special characters are not allowed.
   */
  private readonly nameRegex = /^[A-Za-zÁÉÍÓÚáéíóúÑñ]+$/;

  /**
   * Regular expression for special abilities.
   *
   * Only letters and spaces are allowed.
   */
  private readonly abilityRegex = /^[A-Za-zÁÉÍÓÚáéíóúÑñ ]+$/;

  /**
   * Injects the service used to create and update species.
   */
  constructor(private speciesService: SpeciesService) { }

  /**
   * Allows only letters in the species name field.
   *
   * Navigation keys and common clipboard shortcuts are still allowed.
   *
   * @param event Keyboard event raised by the input.
   */
  allowOnlyLettersNoSpaces(event: KeyboardEvent): void {
    const allowedKeys = [
      'Backspace',
      'Tab',
      'ArrowLeft',
      'ArrowRight',
      'Delete',
      'Home',
      'End'
    ];

    if (allowedKeys.indexOf(event.key) !== -1) {
      return;
    }

    if (
      (event.ctrlKey || event.metaKey) &&
      ['a', 'c', 'v', 'x'].indexOf(event.key.toLowerCase()) !== -1
    ) {
      return;
    }

    if (!this.nameRegex.test(event.key)) {
      event.preventDefault();
    }
  }

  /**
   * Allows only letters and spaces in the special ability field.
   *
   * @param event Keyboard event raised by the textarea.
   */
  allowOnlyLettersAndSpaces(event: KeyboardEvent): void {
    const allowedKeys = [
      'Backspace',
      'Tab',
      'ArrowLeft',
      'ArrowRight',
      'Delete',
      'Home',
      'End'
    ];

    if (allowedKeys.indexOf(event.key) !== -1) {
      return;
    }

    if (
      (event.ctrlKey || event.metaKey) &&
      ['a', 'c', 'v', 'x'].indexOf(event.key.toLowerCase()) !== -1
    ) {
      return;
    }

    if (!/^[A-Za-zÁÉÍÓÚáéíóúÑñ ]$/.test(event.key)) {
      event.preventDefault();
    }
  }

  /**
   * Allows only numeric digits in the power level field.
   *
   * It also prevents typing more than the configured maximum number of digits.
   *
   * @param event Keyboard event raised by the input.
   */
  allowOnlyIntegerKeys(event: KeyboardEvent): void {
    const allowedKeys = [
      'Backspace',
      'Tab',
      'ArrowLeft',
      'ArrowRight',
      'Delete',
      'Home',
      'End'
    ];

    if (allowedKeys.indexOf(event.key) !== -1) {
      return;
    }

    if (
      (event.ctrlKey || event.metaKey) &&
      ['a', 'c', 'v', 'x'].indexOf(event.key.toLowerCase()) !== -1
    ) {
      return;
    }

    if (this.powerLevelInput.length >= this.maxPowerLevelDigits) {
      event.preventDefault();
      return;
    }

    if (!/^[0-9]$/.test(event.key)) {
      event.preventDefault();
    }
  }

  /**
   * Prevents pasting invalid content into the species name field.
   *
   * @param event Clipboard event raised by the input.
   */
  onNamePaste(event: ClipboardEvent): void {
    const clipboardData = event.clipboardData || (window as any).clipboardData;
    const pastedText = clipboardData.getData('text');

    if (!this.nameRegex.test(pastedText)) {
      event.preventDefault();
    }
  }

  /**
   * Prevents pasting invalid content into the special ability field.
   *
   * @param event Clipboard event raised by the textarea.
   */
  onSpecialAbilityPaste(event: ClipboardEvent): void {
    const clipboardData = event.clipboardData || (window as any).clipboardData;
    const pastedText = clipboardData.getData('text');

    if (!this.abilityRegex.test(pastedText)) {
      event.preventDefault();
    }
  }

  /**
   * Prevents pasting invalid content into the power level field.
   *
   * @param event Clipboard event raised by the input.
   */
  onPowerLevelPaste(event: ClipboardEvent): void {
    const clipboardData = event.clipboardData || (window as any).clipboardData;
    const pastedText = clipboardData.getData('text');

    if (!/^[1-9][0-9]{0,9}$/.test(pastedText)) {
      event.preventDefault();
    }
  }

  /**
   * Sanitizes the species name by removing invalid characters and enforcing max length.
   */
  sanitizeSpeciesName(): void {
    this.species.name = (this.species.name || '')
      .replace(/[^A-Za-zÁÉÍÓÚáéíóúÑñ]/g, '')
      .substring(0, 100);
  }

  /**
   * Sanitizes the special ability by removing invalid characters and normalizing spaces.
   */
  sanitizeSpecialAbility(): void {
    this.species.specialAbility = (this.species.specialAbility || '')
      .replace(/[^A-Za-zÁÉÍÓÚáéíóúÑñ ]/g, '')
      .replace(/\s+/g, ' ')
      .substring(0, 100);
  }

  /**
   * Sanitizes and converts the power level input to a numeric value.
   */
  sanitizePowerLevel(): void {
    this.powerLevelInput = (this.powerLevelInput || '')
      .replace(/\D/g, '')
      .substring(0, this.maxPowerLevelDigits);

    if (this.powerLevelInput.startsWith('0')) {
      this.powerLevelInput = this.powerLevelInput.replace(/^0+/, '');
    }

    this.species.powerLevel = this.powerLevelInput
      ? parseInt(this.powerLevelInput, 10)
      : 0;
  }

  /**
   * Indicates whether the selected power level exceeds the backend int limit.
   *
   * @returns True when the value is greater than the maximum supported value.
   */
  isPowerLevelTooLarge(): boolean {
    return this.species.powerLevel > this.maxPowerLevelValue;
  }

  /**
   * Validates whether the current species name already exists in the list.
   *
   * In edit mode, the current species is excluded from the comparison.
   *
   * @returns True when another species already uses the same name.
   */
  isNameDuplicated(): boolean {
    const normalizedName = this.normalizeText(this.species.name);

    if (!normalizedName) {
      return false;
    }

    return this.speciesList.some((item: Species) =>
      this.normalizeText(item.name) === normalizedName &&
      item.id !== this.editingSpeciesId
    );
  }

  /**
   * Saves the form data by creating a new species or updating an existing one.
   *
   * The method sanitizes the form fields, validates business rules in the UI,
   * and then sends the corresponding request to the API.
   *
   * @param form Angular template-driven form instance.
   */
  saveSpecies(form: NgForm): void {
    this.errorMessage = '';
    this.successMessage = '';

    this.sanitizeSpeciesName();
    this.sanitizePowerLevel();
    this.sanitizeSpecialAbility();

    this.species.name = this.species.name.trim();
    this.species.specialAbility = this.species.specialAbility.trim();

    if (
      form.invalid ||
      !this.nameRegex.test(this.species.name) ||
      !this.abilityRegex.test(this.species.specialAbility) ||
      this.species.name.length > 100 ||
      this.species.specialAbility.length > 100 ||
      this.species.powerLevel <= 0 ||
      this.isPowerLevelTooLarge() ||
      this.isNameDuplicated()
    ) {
      this.errorMessage = 'SPECIES_FORM.ERROR_INVALID_FORM';

      Object.keys(form.controls).forEach(function (key) {
        form.controls[key].markAsTouched();
      });

      return;
    }

    this.loading = true;

    if (this.isEditMode && this.editingSpeciesId !== null) {
      const updateRequest: UpdateSpecies = {
        name: this.species.name,
        powerLevel: this.species.powerLevel,
        specialAbility: this.species.specialAbility
      };

      this.speciesService.update(this.editingSpeciesId, updateRequest).subscribe(
        (updatedSpecies: Species) => {
          this.successMessage = 'SPECIES_FORM.SUCCESS_UPDATED';
          this.speciesUpdated.emit(updatedSpecies);
          this.clearForm(form);
          this.loading = false;
        },
        (error: HttpErrorResponse) => {
          this.handleSaveError(error);
        }
      );

      return;
    }

    this.speciesService.create(this.species).subscribe(
      (createdSpecies: Species) => {
        this.successMessage = 'SPECIES_FORM.SUCCESS_CREATED';
        this.speciesCreated.emit(createdSpecies);
        this.clearForm(form);
        this.loading = false;
      },
      (error: HttpErrorResponse) => {
        this.handleSaveError(error);
      }
    );
  }

  /**
   * Enables edit mode and loads the selected species data into the form.
   *
   * @param species Species selected from the list.
   */
  startEdit(species: Species): void {
    this.errorMessage = '';
    this.successMessage = '';

    this.isEditMode = true;
    this.editingSpeciesId = species.id;

    this.species = {
      name: species.name,
      powerLevel: species.powerLevel,
      specialAbility: species.specialAbility
    };

    this.powerLevelInput = species.powerLevel.toString();
  }

  /**
   * Cancels edit mode and clears the form.
   *
   * @param form Angular template-driven form instance.
   */
  cancelEdit(form: NgForm): void {
    this.clearForm(form);
  }

  /**
   * Resets the form model and UI state.
   *
   * @param form Angular template-driven form instance.
   */
  private clearForm(form: NgForm): void {
    this.species = {
      name: '',
      powerLevel: 0,
      specialAbility: ''
    };

    this.powerLevelInput = '';
    this.isEditMode = false;
    this.editingSpeciesId = null;

    form.resetForm({
      name: '',
      powerLevel: '',
      specialAbility: ''
    });
  }

  /**
   * Maps API errors to translation keys used by the template.
   *
   * @param error Error returned by HttpClient.
   */
  private handleSaveError(error: HttpErrorResponse): void {
    if (error.status === 409) {
      this.errorMessage = 'SPECIES_FORM.ERROR_DUPLICATED_NAME';
    } else if (error.status === 400) {
      this.errorMessage = 'SPECIES_FORM.ERROR_BAD_REQUEST';
    } else {
      this.errorMessage = 'SPECIES_FORM.ERROR_SAVE';
    }

    this.loading = false;
  }

  /**
   * Normalizes text for case-insensitive comparisons.
   *
   * @param value Text value to normalize.
   * @returns Lowercase trimmed text.
   */
  private normalizeText(value: string): string {
    return (value || '').trim().toLowerCase();
  }
}
