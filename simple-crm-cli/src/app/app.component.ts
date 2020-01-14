import { Component } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { LayoutState, toggleSidenav, selectShowSideNav } from './store/layout.store';
import { Observable } from 'rxjs';

@Component({
  selector: 'crm-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  showSideNav$: Observable<boolean>;

  constructor(private store: Store<LayoutState>) {
    this.showSideNav$ = this.store.pipe(select(selectShowSideNav));
  }

  sideNavToggle() {
    this.store.dispatch(toggleSidenav());
  }
}
