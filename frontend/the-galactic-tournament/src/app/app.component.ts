import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

/**
 * Root component of the Angular application.
 *
 * Responsibilities:
 * - Render the main application layout.
 * - Provide the global navigation menu.
 * - Configure and manage the active application language.
 * - Persist the selected language in localStorage.
 */
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  /**
   * Application title used as a simple component-level identifier.
   */
  title = 'The Galactic Tournament';

  /**
   * Current language selected by the user.
   *
   * Supported values:
   * - en: English.
   * - es: Spanish.
   */
  currentLanguage = 'en';

  /**
   * Initializes the translation service and restores the previously selected language.
   *
   * If no language was stored, English is used by default.
   */
  constructor(private translate: TranslateService) {
    this.translate.addLangs(['en', 'es']);
    this.translate.setDefaultLang('en');

    const savedLanguage = localStorage.getItem('language') || 'en';

    this.currentLanguage = savedLanguage;
    this.translate.use(savedLanguage);
  }

  /**
   * Changes the active application language.
   *
   * The selected language is also stored in localStorage so it can be restored
   * when the user reloads or opens the application again.
   *
   * @param language Language code to apply.
   */
  changeLanguage(language: string): void {
    this.currentLanguage = language;
    this.translate.use(language);
    localStorage.setItem('language', language);
  }
}
