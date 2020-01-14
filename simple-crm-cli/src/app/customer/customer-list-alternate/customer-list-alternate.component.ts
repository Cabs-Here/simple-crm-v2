import { Component, OnInit, ChangeDetectionStrategy, Input } from '@angular/core';
import { Customer } from '../customer.model';
import { Router } from '@angular/router';

@Component({
  selector: 'crm-customer-list-alternate',
  templateUrl: './customer-list-alternate.component.html',
  styleUrls: ['./customer-list-alternate.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default
})
export class CustomerListAlternateComponent implements OnInit {

  @Input() customers: Customer[];
  displayColumns = ['icon', 'name', 'phone', 'email', 'lastContactDate', 'status', 'actions'];

  constructor(private router: Router) {

  }

  ngOnInit() {
  }

  trackByCustomerId(cust: Customer) {
    return cust.customerId;
  }
  openDetail(item: Customer) {
    if (item) {
      this.router.navigate([`./customer/${item.customerId}`]);
    }
  }

}
