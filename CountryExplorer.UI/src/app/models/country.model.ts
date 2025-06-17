export interface Country {
  name: {
    common: string;
    official: string;
  };
  capital: string[];
  region: string;
  subregion: string;
  population: number;
  languages: { [key: string]: string };
  flags: {
    png: string;
    svg: string;
  };
  area: number;
  currencies: { [key: string]: { name: string; symbol: string } };
  cca2: string;
}

export interface Weather {
  temperature: number;
  description: string;
  icon: string;
  humidity: number;
  windSpeed: number;
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}
