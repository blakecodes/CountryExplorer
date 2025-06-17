import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { NzEmptyModule } from 'ng-zorro-antd/empty';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzStatisticModule } from 'ng-zorro-antd/statistic';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { CountryService } from '../../services/country.service';
import { Country } from '../../models/country.model';
import { CountryDetailComponent } from '../country-detail/country-detail.component';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-country-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    NzCardModule,
    NzGridModule,
    NzInputModule,
    NzSelectModule,
    NzSpinModule,
    NzEmptyModule,
    NzTagModule,
    NzIconModule,
    NzStatisticModule,
    NzModalModule,
    CountryDetailComponent
  ],
  templateUrl: './country-list.component.html',
  styleUrl: './country-list.component.scss'
})
export class CountryListComponent implements OnInit {
  countries: Country[] = [];
  filteredCountries: Country[] = [];
  regions: string[] = [];
  selectedRegion: string = '';
  searchTerm: string = '';
  loading: boolean = false;
  selectedCountry: Country | null = null;
  isDetailModalVisible: boolean = false;
  private searchSubject = new Subject<string>();

  constructor(private countryService: CountryService) {}

  ngOnInit(): void {
    this.loadCountries();
    this.loadRegions();
    this.setupSearch();
  }

  loadRegions(): void {
    this.countryService.getRegions().subscribe(regions => {
      this.regions = regions;
    });
  }

  loadCountries(): void {
    this.loading = true;
    this.countryService.getAllCountries().subscribe({
      next: (countries) => {
        this.countries = countries;
        this.applyFilters();
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading countries:', error);
        this.loading = false;
      }
    });
  }

  onSearch(value: string): void {
    this.searchTerm = value;
    this.applyFilters();
  }

  onRegionChange(region: string | null): void {
    this.selectedRegion = region || '';
    this.applyFilters();
  }

  applyFilters(): void {
    let filtered = [...this.countries];

    if (this.searchTerm) {
      filtered = filtered.filter(country =>
        country.name.common.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        country.name.official.toLowerCase().includes(this.searchTerm.toLowerCase())
      );
    }

    if (this.selectedRegion) {
      filtered = filtered.filter(country => country.region === this.selectedRegion);
    }

    this.filteredCountries = filtered;
  }

  showCountryDetail(country: Country): void {
    this.selectedCountry = country;
    this.isDetailModalVisible = true;
  }

  handleModalClose(): void {
    this.isDetailModalVisible = false;
    this.selectedCountry = null;
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

  getLanguagesList(languages: { [key: string]: string }): string {
    return Object.values(languages).join(', ');
  }

  setupSearch(): void {
    this.searchSubject.subscribe(term => {
      this.searchTerm = term;
      this.applyFilters();
    });
  }
}
