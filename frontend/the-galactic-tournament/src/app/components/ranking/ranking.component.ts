import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

import { RankingItem } from '../../models/ranking-item';
import { RankingService } from '../../services/ranking.service';

/**
 * Component responsible for displaying and managing the tournament ranking.
 *
 * Responsibilities:
 * - Load the current ranking from the API.
 * - Display species ordered by victories.
 * - Allow the user to reset the ranking.
 * - Show translated success and error messages.
 */
@Component({
  selector: 'app-ranking',
  templateUrl: './ranking.component.html',
  styleUrls: ['./ranking.component.scss']
})
export class RankingComponent implements OnInit {

  /**
   * Ranking items returned by the API.
   */
  ranking: RankingItem[] = [];

  /**
   * Indicates whether a ranking request is currently running.
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
   * Injects the ranking service and translation service.
   */
  constructor(
    private rankingService: RankingService,
    private translate: TranslateService
  ) { }

  /**
   * Loads the ranking when the component is initialized.
   */
  ngOnInit(): void {
    this.loadRanking();
  }

  /**
   * Loads the current tournament ranking from the API.
   */
  loadRanking(): void {
    this.loading = true;
    this.errorMessage = '';

    this.rankingService.getRanking().subscribe(
      (data: RankingItem[]) => {
        this.ranking = data;
        this.loading = false;
      },
      (error) => {
        this.errorMessage = this.getApiErrorMessage(error, 'RANKING.ERROR_LOADING');
        this.loading = false;
      }
    );
  }

  /**
   * Resets the tournament ranking after user confirmation.
   *
   * The backend resets the ranking by removing battle history while keeping
   * the registered species unchanged.
   */
  resetRanking(): void {
    this.errorMessage = '';
    this.successMessage = '';

    const confirmed = confirm(this.translate.instant('RANKING.CONFIRM_RESET'));

    if (!confirmed) {
      return;
    }

    this.loading = true;

    this.rankingService.resetRanking().subscribe(
      () => {
        this.successMessage = 'RANKING.SUCCESS_RESET';
        this.loadRanking();
      },
      (error) => {
        this.errorMessage = this.getApiErrorMessage(error, 'RANKING.ERROR_RESET');
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
}
