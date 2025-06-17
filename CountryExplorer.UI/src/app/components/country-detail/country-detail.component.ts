import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NzDescriptionsModule } from 'ng-zorro-antd/descriptions';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzStatisticModule } from 'ng-zorro-antd/statistic';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzSwitchModule } from 'ng-zorro-antd/switch';
import { FormsModule } from '@angular/forms';
import { Country, Weather } from '../../models/country.model';
import { CountryService } from '../../services/country.service';

@Component({
  selector: 'app-country-detail',
  standalone: true,
  imports: [
    CommonModule,
    NzDescriptionsModule,
    NzTagModule,
    NzDividerModule,
    NzStatisticModule,
    NzGridModule,
    NzCardModule,
    NzSpinModule,
    NzIconModule,
    NzSwitchModule,
    FormsModule
  ],
  templateUrl: './country-detail.component.html',
  styleUrls: ['./country-detail.component.scss']
})
export class CountryDetailComponent implements OnInit {
  @Input() country!: Country;
  weather: Weather | null = null;
  loadingWeather = false;
  isFahrenheit = false;

  constructor(private countryService: CountryService) {}

  ngOnInit(): void {
    if (this.country && this.country.capital && this.country.capital.length > 0) {
      this.loadingWeather = true;
      this.countryService.getWeatherForCapital(this.country.capital[0], this.country.cca2).subscribe(
        weather => {
          this.weather = weather;
          this.loadingWeather = false;
        },
        error => {
          console.error('Error loading weather:', error);
          this.loadingWeather = false;
        }
      );
    }
  }

  getDisplayTemperature(): number {
    if (!this.weather) return 0;
    if (this.isFahrenheit) {
      return Math.round((this.weather.temperature * 9/5 + 32) * 10) / 10;
    }
    return Math.round(this.weather.temperature * 10) / 10;
  }

  getTemperatureUnit(): string {
    return this.isFahrenheit ? '°F' : '°C';
  }

  getLanguagesList(): string {
    if (!this.country.languages) return 'N/A';
    return Object.values(this.country.languages).join(', ');
  }

  getCurrenciesList(): string {
    if (!this.country.currencies) return 'N/A';
    return Object.entries(this.country.currencies)
      .map(([code, currency]) => `${currency.name} (${currency.symbol || code})`)
      .join(', ');
  }

  formatPopulation(population: number): string {
    if (population >= 1000000000) {
      return (population / 1000000000).toFixed(2) + 'B';
    } else if (population >= 1000000) {
      return (population / 1000000).toFixed(2) + 'M';
    } else if (population >= 1000) {
      return (population / 1000).toFixed(2) + 'K';
    }
    return population.toString();
  }

  formatArea(area: number): string {
    return area.toLocaleString() + ' km²';
  }

  getWeatherIcon(icon: string): string {
    return `https://openweathermap.org/img/wn/${icon}@2x.png`;
  }
}
