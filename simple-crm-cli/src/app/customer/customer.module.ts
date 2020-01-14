
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CustomerRoutingModule } from './customer-routing.module';
import { CustomerListPageComponent } from './customer-list-page/customer-list-page.component';
import { SharedImportsModule } from '../shared/shared-imports.module';
import { HttpClientModule } from '@angular/common/http';
import { CustomerService } from './customer.service';
import { CustomerMockService } from './customer-mock.service';
import { environment } from 'src/environments/environment';
import { CustomerCreateDialogComponent } from './customer-create-dialog/customer-create-dialog.component';
import { ReactiveFormsModule } from '@angular/forms';
import { CustomerDetailComponent } from './customer-detail/customer-detail.component';
import { StatusIconPipe } from './status-icon.pipe';
import { StoreModule } from '@ngrx/store';
import { customerFeatureKey, customerReducer } from './store/customer.store';
import { EffectsModule } from '@ngrx/effects';
import { CustomerStoreEffects } from './store/customer.store.effects';
import { CustomerListAlternateComponent } from './customer-list-alternate/customer-list-alternate.component';


@NgModule({
  declarations: [
    CustomerListPageComponent,
    CustomerListAlternateComponent,
    CustomerCreateDialogComponent,
    CustomerDetailComponent,
    StatusIconPipe
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    HttpClientModule,
    CustomerRoutingModule,
    SharedImportsModule,
    /**
     * StoreModule.forFeature is used for composing state
     * from feature modules. These modules can be loaded
     * eagerly or lazily and will be dynamically added to
     * the existing state.
     */
    StoreModule.forFeature(customerFeatureKey, customerReducer),
    EffectsModule.forFeature([CustomerStoreEffects])
  ],
  providers: [
    CustomerService
  ],
  entryComponents: [
    CustomerCreateDialogComponent
  ]
})
export class CustomerModule { }
