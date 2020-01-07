import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CustomerListPageComponent } from './customer-list-page/customer-list-page.component';
import { CustomerDetailComponent } from './customer-detail/customer-detail.component';
import { AuthenticatedGuard } from '../account/authenticated.guard';
import { CustomerListAlternateComponent } from './customer-list-alternate/customer-list-alternate.component';


const routes: Routes = [
{
  path: 'customers',
  pathMatch: 'full',
  component: CustomerListAlternateComponent,
  canActivate: [AuthenticatedGuard]
},
{
  path: 'customer/:id',
  pathMatch: 'full',
  component: CustomerDetailComponent,
  canActivate: [AuthenticatedGuard]
}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CustomerRoutingModule { }
