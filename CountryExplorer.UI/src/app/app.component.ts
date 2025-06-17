import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { CountryListComponent } from './components/country-list/country-list.component';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    NzLayoutModule,
    NzMenuModule,
    NzIconModule,
    CountryListComponent
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'Country Explorer';
  isCollapsed = false;
}
