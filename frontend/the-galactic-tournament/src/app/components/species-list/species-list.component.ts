import { Component, OnInit, ViewChild } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

import { Species } from '../../models/species';
import { SpeciesService } from '../../services/species.service';
import { SpeciesFormComponent } from '../species-form/species-form.component';

/**
 * Component responsible for displaying and managing the species list.
 *
 * Responsibilities:
 * - Load species from the API.
 * - Display species in a filterable and sortable table.
 * - Start species edition through the child form component.
 * - Delete species after user confirmation.
 * - Show translated success and error messages.
 */
@Component({
  selector: 'app-species-list',
  templateUrl: './species-list.component.html',
  styleUrls: ['./species-list.component.scss']
})
export class SpeciesListComponent implements OnInit {

  /**
   * Reference to the child form component.
   *
   * It is used to call startEdit when the user clicks the Edit button
   * in the species table.
   */
  @ViewChild(SpeciesFormComponent, { static: false })
  speciesFormComponent!: SpeciesFormComponent;

  /**
   * Full list of species loaded from the API.
   */
  speciesList: Species[] = [];

  /**
   * Indicates whether a list or delete request is currently running.
   */
  loading = false;

  /**
   * Translation key or API message displayed when an error occurs.
   */
  errorMessage = '';

  /**
   * Translation key displayed when an operation completes successfully.
   */
  successMessage = '';

  /**
   * Filters used by the table.
   *
   * Each property is bound to an input in the filter row.
   */
  filters = {
    id: '',
    name: '',
    powerLevel: '',
    specialAbility: '',
    createdAt: ''
  };

  /**
   * Current column used to sort the table.
   */
  sortColumn = 'id';

  /**
   * Current sorting direction.
   */
  sortDirection: 'asc' | 'desc' = 'asc';

  /**
   * Injects the species service and translation service.
   */
  constructor(
    private speciesService: SpeciesService,
    private translate: TranslateService
  ) { }

  /**
   * Loads the species list when the component is initialized.
   */
  ngOnInit(): void {
    this.loadSpecies();
  }

  /**
   * Returns the species list after applying filters and sorting.
   *
   * This getter is used directly by the template to keep the table updated
   * when filters or sorting options change.
   */
  get filteredAndSortedSpeciesList(): Species[] {
    var filtered = this.speciesList.filter((item: Species) => {
      return this.containsValue(item.id, this.filters.id) &&
        this.containsValue(item.name, this.filters.name) &&
        this.containsValue(item.powerLevel, this.filters.powerLevel) &&
        this.containsValue(item.specialAbility, this.filters.specialAbility) &&
        this.containsValue(this.formatDateForFilter(item.createdAt), this.filters.createdAt);
    });

    return filtered.sort((a: Species, b: Species) => this.compareSpecies(a, b));
  }

  /**
   * Loads all species from the API.
   */
  loadSpecies(): void {
    this.loading = true;
    this.errorMessage = '';

    this.speciesService.getAll().subscribe(
      (data: Species[]) => {
        this.speciesList = data;
        this.loading = false;
      },
      (error) => {
        this.errorMessage = this.getApiErrorMessage(error, 'SPECIES_LIST.ERROR_LOADING');
        this.loading = false;
      }
    );
  }

  /**
   * Handles the event emitted by the form after a species is created.
   */
  onSpeciesCreated(): void {
    this.successMessage = 'SPECIES_LIST.SUCCESS_CREATED';
    this.loadSpecies();
  }

  /**
   * Handles the event emitted by the form after a species is updated.
   */
  onSpeciesUpdated(): void {
    this.successMessage = 'SPECIES_LIST.SUCCESS_UPDATED';
    this.loadSpecies();
  }

  /**
   * Sends the selected species to the form component to enable edit mode.
   *
   * @param species Species selected by the user.
   */
  editSpecies(species: Species): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (this.speciesFormComponent) {
      this.speciesFormComponent.startEdit(species);
    }
  }

  /**
   * Deletes a species after user confirmation.
   *
   * If the backend blocks the delete operation because the species has battle history,
   * the API error is mapped to a translation key.
   *
   * @param species Species selected by the user.
   */
  deleteSpecies(species: Species): void {
    this.errorMessage = '';
    this.successMessage = '';

    const confirmed = confirm(this.translate.instant('SPECIES_LIST.CONFIRM_DELETE'));

    if (!confirmed) {
      return;
    }

    this.loading = true;

    this.speciesService.delete(species.id).subscribe(
      () => {
        this.successMessage = 'SPECIES_LIST.SUCCESS_DELETED';
        this.loadSpecies();
      },
      (error) => {
        this.errorMessage = this.getApiErrorMessage(error, 'SPECIES_LIST.ERROR_DELETE');
        this.loading = false;
      }
    );
  }

  /**
   * Changes the active sort column.
   *
   * If the same column is clicked twice, the sort direction is toggled.
   *
   * @param column Column name to sort by.
   */
  sortBy(column: string): void {
    if (this.sortColumn === column) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
      return;
    }

    this.sortColumn = column;
    this.sortDirection = 'asc';
  }

  /**
   * Returns the visual icon for the current sorting state.
   *
   * @param column Column name to evaluate.
   * @returns Sort icon for inactive, ascending or descending state.
   */
  getSortIcon(column: string): string {
    if (this.sortColumn !== column) {
      return '↕';
    }

    return this.sortDirection === 'asc' ? '▲' : '▼';
  }

  /**
   * Clears all table filters.
   */
  clearFilters(): void {
    this.filters = {
      id: '',
      name: '',
      powerLevel: '',
      specialAbility: '',
      createdAt: ''
    };
  }

  /**
   * Indicates whether any table filter is currently active.
   *
   * @returns True when at least one filter has a value.
   */
  hasActiveFilters(): boolean {
    return !!(
      this.filters.id ||
      this.filters.name ||
      this.filters.powerLevel ||
      this.filters.specialAbility ||
      this.filters.createdAt
    );
  }

  /**
   * Compares two species according to the active sort column and direction.
   *
   * @param a First species.
   * @param b Second species.
   * @returns Comparison result used by Array.sort.
   */
  private compareSpecies(a: Species, b: Species): number {
    var aValue = this.getSortableValue(a, this.sortColumn);
    var bValue = this.getSortableValue(b, this.sortColumn);
    var result = 0;

    if (typeof aValue === 'number' && typeof bValue === 'number') {
      result = aValue - bValue;
    } else {
      result = aValue.toString().localeCompare(bValue.toString());
    }

    return this.sortDirection === 'asc' ? result : result * -1;
  }

  /**
   * Gets the value used to sort a species by a given column.
   *
   * @param item Species to evaluate.
   * @param column Column name.
   * @returns Sortable value for the selected column.
   */
  private getSortableValue(item: Species, column: string): any {
    switch (column) {
      case 'id':
        return item.id;
      case 'name':
        return item.name || '';
      case 'powerLevel':
        return item.powerLevel;
      case 'specialAbility':
        return item.specialAbility || '';
      case 'createdAt':
        return new Date(item.createdAt).getTime();
      default:
        return item.id;
    }
  }

  /**
   * Evaluates whether a value contains the provided filter text.
   *
   * @param value Original value to compare.
   * @param filter Filter entered by the user.
   * @returns True when the value matches the filter.
   */
  private containsValue(value: any, filter: string): boolean {
    if (!filter) {
      return true;
    }

    return value
      .toString()
      .toLowerCase()
      .indexOf(filter.toLowerCase()) !== -1;
  }

  /**
   * Builds a searchable date string for the Created column filter.
   *
   * @param value Date value received from the API.
   * @returns Combined raw and localized date string.
   */
  private formatDateForFilter(value: string): string {
    if (!value) {
      return '';
    }

    return value + ' ' + new Date(value).toLocaleString();
  }

  /**
   * Extracts a readable error message from an API response.
   *
   * @param error Error object returned by HttpClient.
   * @param fallback Translation key used when the API does not return a message.
   * @returns API message mapped to translation key when possible.
   */
  private getApiErrorMessage(error: any, fallback: string): string {
    const apiMessage = this.extractApiMessage(error);

    if (apiMessage) {
      return this.mapApiMessage(apiMessage);
    }

    return fallback;
  }

  /**
   * Extracts the raw message returned by the API.
   *
   * @param error Error object returned by HttpClient.
   * @returns API error message or empty string.
   */
  private extractApiMessage(error: any): string {
    if (error && error.error && error.error.message) {
      return error.error.message;
    }

    if (error && typeof error.error === 'string') {
      return error.error;
    }

    return '';
  }

  /**
   * Maps known backend messages to frontend translation keys.
   *
   * @param message API message to map.
   * @returns Translation key when known; otherwise, the original message.
   */
  private mapApiMessage(message: string): string {
    if (message === 'This species cannot be deleted because it already has battle history.') {
      return 'API_ERRORS.SPECIES_DELETE_WITH_HISTORY';
    }

    return message;
  }
}
