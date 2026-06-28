import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

import { BattleResult, StartBattle } from '../../models/battle';
import { Species } from '../../models/species';
import { BattleService } from '../../services/battle.service';
import { SpeciesService } from '../../services/species.service';

/**
 * Component responsible for managing battles from the frontend.
 *
 * Responsibilities:
 * - Load registered species.
 * - Load battle history.
 * - Start a battle between two selected species.
 * - Start a random battle.
 * - Display the latest battle result.
 * - Translate known battle reasons returned by the API.
 */
@Component({
  selector: 'app-battle-form',
  templateUrl: './battle-form.component.html',
  styleUrls: ['./battle-form.component.scss']
})
export class BattleFormComponent implements OnInit {

  /**
   * Species available to be selected for battles.
   */
  speciesList: Species[] = [];

  /**
   * Battle history returned by the API.
   */
  battles: BattleResult[] = [];

  /**
   * Identifier of the first selected species.
   */
  speciesAId = 0;

  /**
   * Identifier of the second selected species.
   */
  speciesBId = 0;

  /**
   * Most recent battle result shown after starting a battle.
   */
  lastBattle: BattleResult | null = null;

  /**
   * Indicates whether a battle request is currently running.
   */
  loading = false;

  /**
   * Translation key or API message displayed when an error occurs.
   */
  errorMessage = '';

  /**
   * Injects the services required to load species, manage battles and translate messages.
   */
  constructor(
    private speciesService: SpeciesService,
    private battleService: BattleService,
    private translate: TranslateService
  ) { }

  /**
   * Loads the initial data required by the component.
   */
  ngOnInit(): void {
    this.loadSpecies();
    this.loadBattles();
  }

  /**
   * Loads all registered species from the API.
   */
  loadSpecies(): void {
    this.speciesService.getAll().subscribe(
      (data: Species[]) => {
        this.speciesList = data;
      },
      (error) => {
        this.errorMessage = this.getApiErrorMessage(error, 'BATTLE.ERROR_LOADING_SPECIES');
      }
    );
  }

  /**
   * Loads the battle history from the API.
   */
  loadBattles(): void {
    this.battleService.getAll().subscribe(
      (data: BattleResult[]) => {
        this.battles = data;
      },
      (error) => {
        this.errorMessage = this.getApiErrorMessage(error, 'BATTLE.ERROR_LOADING_BATTLES');
      }
    );
  }

  /**
   * Starts a battle using the two species selected by the user.
   *
   * The method validates that both species were selected and that they are different
   * before sending the request to the API.
   */
  startBattle(): void {
    this.errorMessage = '';
    this.lastBattle = null;

    const speciesAId = Number(this.speciesAId);
    const speciesBId = Number(this.speciesBId);

    if (speciesAId <= 0 || speciesBId <= 0) {
      this.errorMessage = 'BATTLE.ERROR_SELECT_TWO';
      return;
    }

    if (speciesAId === speciesBId) {
      this.errorMessage = 'BATTLE.ERROR_SAME_SPECIES';
      return;
    }

    const request: StartBattle = {
      speciesAId: speciesAId,
      speciesBId: speciesBId
    };

    this.executeBattle(() => this.battleService.startBattle(request));
  }

  /**
   * Starts a battle using two random species selected by the backend.
   */
  startRandomBattle(): void {
    this.errorMessage = '';
    this.lastBattle = null;
    this.executeBattle(() => this.battleService.startRandomBattle());
  }

  /**
   * Translates a battle result reason.
   *
   * The API may return either a translation key or a plain text message.
   * This method supports both scenarios.
   *
   * @param reason Reason returned by the API.
   * @returns Translated reason when available; otherwise, the original reason.
   */
  translateBattleReason(reason: string): string {
    if (!reason) {
      return '';
    }

    const translated = this.translate.instant(reason);

    if (translated !== reason) {
      return translated;
    }

    return this.translateKnownBattleReason(reason);
  }

  /**
   * Executes a battle request and updates the UI state.
   *
   * This method centralizes the loading and error handling logic used by both
   * manual battles and random battles.
   *
   * @param action Function that returns the battle request observable.
   */
  private executeBattle(action: () => any): void {
    this.loading = true;

    action().subscribe(
      (result: BattleResult) => {
        this.lastBattle = result;
        this.loading = false;
        this.loadBattles();
      },
      (error) => {
        this.errorMessage = this.getApiErrorMessage(error, 'BATTLE.ERROR_STARTING');
        this.loading = false;
      }
    );
  }

  /**
   * Extracts a readable error message from an API response.
   *
   * @param error Error object returned by HttpClient.
   * @param fallback Translation key used when the API does not return a message.
   * @returns API message or fallback translation key.
   */
  private getApiErrorMessage(error: any, fallback: string): string {
    if (error && error.error && error.error.message) {
      return error.error.message;
    }

    if (error && typeof error.error === 'string') {
      return error.error;
    }

    return fallback;
  }

  /**
   * Maps known plain text battle reasons to translation keys.
   *
   * This helps translate backend messages that are not returned as translation keys.
   *
   * @param reason Plain text reason returned by the API.
   * @returns Translated known reason or the original reason.
   */
  private translateKnownBattleReason(reason: string): string {
    const lowerReason = reason.toLowerCase();

    if (lowerReason.indexOf('higher power') !== -1 || lowerReason.indexOf('highest power') !== -1) {
      return this.translate.instant('BATTLE.REASON_HIGHER_POWER');
    }

    if (lowerReason.indexOf('alphabet') !== -1) {
      return this.translate.instant('BATTLE.REASON_ALPHABETICAL');
    }

    return reason;
  }
}
