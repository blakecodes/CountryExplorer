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
import { NzPaginationModule } from 'ng-zorro-antd/pagination';
import { CountryService } from '../../services/country.service';
import { Country, PagedResult } from '../../models/country.model';
import { CountryDetailComponent } from '../country-detail/country-detail.component';
import { Subject, debounceTime, distinctUntilChanged } from 'rxjs';

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
    NzPaginationModule,
    CountryDetailComponent
  ],
  templateUrl: './country-list.component.html',
  styleUrl: './country-list.component.scss'
})
export class CountryListComponent implements OnInit {
  countries: Country[] = [];
  filteredCountries: Country[] = [];
  totalCount: number = 0;
  pageNumber: number = 1;
  pageSize: number = 20;
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
    this.countryService.getAllCountries(this.pageNumber, this.pageSize).subscribe({
      next: (result: PagedResult<Country>) => {
        this.countries = result.items;
        this.totalCount = result.totalCount;
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
    this.pageNumber = 1;
    this.searchSubject.next(value);
  }

  onRegionChange(region: string | null): void {
    this.selectedRegion = region || '';
    this.pageNumber = 1;
    if (region) {
      this.loading = true;
      this.countryService.getCountriesByRegion(region, this.pageNumber, this.pageSize).subscribe({
        next: (result: PagedResult<Country>) => {
          this.countries = result.items;
          this.totalCount = result.totalCount;
          this.applyFilters();
          this.loading = false;
        },
        error: (error) => {
          console.error('Error loading countries by region:', error);
          this.loading = false;
        }
      });
    } else {
      this.loadCountries();
    }
  }

  applyFilters(): void {
    this.filteredCountries = this.countries;
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
    this.searchSubject.pipe(
      debounceTime(300),
      distinctUntilChanged()
    ).subscribe(term => {
      this.performSearch(term);
    });
  }

  onPageIndexChange(pageIndex: number): void {
    this.pageNumber = pageIndex;
    this.reloadCurrentView();
  }

  onPageSizeChange(size: number): void {
    this.pageSize = size;
    this.pageNumber = 1;
    this.reloadCurrentView();
  }

  private reloadCurrentView(): void {
    if (this.selectedRegion) {
      this.onRegionChange(this.selectedRegion);
    } else if (this.searchTerm.trim()) {
      this.performSearch(this.searchTerm);
    } else {
      this.loadCountries();
    }
  }

  performSearch(searchTerm: string): void {
    if (!searchTerm.trim()) {
      this.loadCountries();
      return;
    }

    this.loading = true;
    this.pageNumber = 1;
    this.countryService.searchCountries(searchTerm, this.pageNumber, this.pageSize).subscribe({
      next: (result: PagedResult<Country>) => {
        this.countries = result.items;
        this.totalCount = result.totalCount;
        this.applyFilters();
        this.loading = false;
      },
      error: (error) => {
        console.error('Error searching countries:', error);
        this.loading = false;
      }
    });
  }
}
