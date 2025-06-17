import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { Country, Weather, PagedResult } from '../models/country.model';

@Injectable({
  providedIn: 'root'
})
export class CountryService {
  private apiUrl = 'http://localhost:5117/api/countries';

  constructor(private http: HttpClient) { }

  getAllCountries(pageNumber: number = 1, pageSize: number = 20): Observable<PagedResult<Country>> {
    const params = new HttpParams().set('pageNumber', pageNumber).set('pageSize', pageSize);
    return this.http.get<any>(this.apiUrl, { params }).pipe(
      map(res => this.mapPagedResult(res)),
      catchError(error => {
        console.error('Error fetching countries:', error);
        return of({ items: [], totalCount: 0, pageNumber, pageSize, totalPages: 0, hasNextPage: false, hasPreviousPage: false } as PagedResult<Country>);
      })
    );
  }

  getCountriesByRegion(region: string, pageNumber: number = 1, pageSize: number = 20): Observable<PagedResult<Country>> {
    const params = new HttpParams().set('pageNumber', pageNumber).set('pageSize', pageSize);
    return this.http.get<any>(`${this.apiUrl}/region/${region}`, { params }).pipe(
      map(res => this.mapPagedResult(res)),
      catchError(error => {
        console.error('Error fetching countries by region:', error);
        return of({ items: [], totalCount: 0, pageNumber, pageSize, totalPages: 0, hasNextPage: false, hasPreviousPage: false } as PagedResult<Country>);
      })
    );
  }

  searchCountries(searchTerm: string, pageNumber: number = 1, pageSize: number = 20): Observable<PagedResult<Country>> {
    const params = new HttpParams()
      .set('name', searchTerm)
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);
    return this.http.get<any>(`${this.apiUrl}/search`, { params }).pipe(
      map(res => this.mapPagedResult(res)),
      catchError(error => {
        console.error('Error searching countries:', error);
        return of({ items: [], totalCount: 0, pageNumber, pageSize, totalPages: 0, hasNextPage: false, hasPreviousPage: false } as PagedResult<Country>);
      })
    );
  }

  getWeatherForCapital(capital: string, countryCode: string): Observable<Weather | null> {
    return this.http.get<Weather>(`${this.apiUrl}/${countryCode}/weather`).pipe(
      catchError(error => {
        console.error('Error fetching weather:', error);
        return of(null);
      })
    );
  }

  getRegions(): Observable<string[]> {
    return this.http.get<string[]>(`${this.apiUrl}/regions`).pipe(
      catchError(error => {
        console.error('Error fetching regions:', error);
        return of([]);
      })
    );
  }

  private mapCountriesToModel(apiCountries: any[]): Country[] {
    return apiCountries.map(country => ({
      name: {
        common: country.name?.common || '',
        official: country.name?.official || ''
      },
      capital: country.capital || [],
      region: country.region || '',
      subregion: country.subregion || '',
      population: country.population || 0,
      languages: country.languages || {},
      flags: {
        png: country.flags?.png || '',
        svg: country.flags?.svg || ''
      },
      area: country.area || 0,
      currencies: country.currencies || {},
      cca2: country.cca2 || ''
    }));
  }

  private mapPagedResult(res: any): PagedResult<Country> {
    // If API returned plain array (fallback) treat whole array as items
    if (Array.isArray(res)) {
      const items = this.mapCountriesToModel(res);
      return { items, totalCount: items.length, pageNumber: 1, pageSize: items.length, totalPages: 1, hasNextPage: false, hasPreviousPage: false };
    }

    const items = this.mapCountriesToModel(res.items || res.Items || []);
    return {
      items,
      totalCount: res.totalCount ?? res.TotalCount ?? items.length,
      pageNumber: res.pageNumber ?? res.PageNumber ?? 1,
      pageSize: res.pageSize ?? res.PageSize ?? items.length,
      totalPages: res.totalPages ?? res.TotalPages ?? 1,
      hasPreviousPage: res.hasPreviousPage ?? res.HasPreviousPage ?? false,
      hasNextPage: res.hasNextPage ?? res.HasNextPage ?? false
    } as PagedResult<Country>;
  }
}
