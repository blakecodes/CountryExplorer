import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { Country, Weather } from '../models/country.model';

@Injectable({
  providedIn: 'root'
})
export class CountryService {
  private apiUrl = 'http://localhost:5117/api/countries';

  constructor(private http: HttpClient) { }

  getAllCountries(): Observable<Country[]> {
    return this.http.get<any[]>(this.apiUrl).pipe(
      map(countries => this.mapCountriesToModel(countries)),
      catchError(error => {
        console.error('Error fetching countries:', error);
        return of([]);
      })
    );
  }

  getCountriesByRegion(region: string): Observable<Country[]> {
    return this.http.get<any[]>(`${this.apiUrl}/region/${region}`).pipe(
      map(countries => this.mapCountriesToModel(countries)),
      catchError(error => {
        console.error('Error fetching countries by region:', error);
        return of([]);
      })
    );
  }

  searchCountries(searchTerm: string): Observable<Country[]> {
    const params = new HttpParams().set('name', searchTerm);
    return this.http.get<any[]>(`${this.apiUrl}/search`, { params }).pipe(
      map(countries => this.mapCountriesToModel(countries)),
      catchError(error => {
        console.error('Error searching countries:', error);
        return of([]);
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
}
