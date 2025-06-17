import {
  MenuFoldOutline,
  MenuUnfoldOutline,
  FormOutline,
  DashboardOutline
} from '@ant-design/icons-angular/icons';
import { provideNzIcons as provideIcons } from 'ng-zorro-antd/icon';

export const icons = [MenuFoldOutline, MenuUnfoldOutline, DashboardOutline, FormOutline];

export function provideNzIcons() {
  return provideIcons(icons);
}
