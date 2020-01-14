import { Component, OnInit } from '@angular/core';
import { Customer } from '../customer.model';
import { MatTableDataSource } from '@angular/material';
import { CustomerService } from '../customer.service';
import { Observable, combineLatest } from 'rxjs';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { CustomerCreateDialogComponent } from '../customer-create-dialog/customer-create-dialog.component';
import { Router } from '@angular/router';
import { FormControl } from '@angular/forms';
import { startWith, debounceTime, map } from 'rxjs/operators';

@Component({
  selector: 'crm-customer-list-alternate',
  templateUrl: './customer-list-alternate.component.html',
  styleUrls: ['./customer-list-alternate.component.scss']
})
export class CustomerListAlternateComponent implements OnInit {
  customers$: Observable<Customer[]>;
  dataSource: MatTableDataSource<Customer>;
  displayColumns = ['statusIcon', 'name', 'phone', 'email', 'lastContactDate', 'status', 'detail'];
  filterInput = new FormControl();


  constructor(private customerService: CustomerService,
              public dialog: MatDialog,
              private router: Router) { }

  ngOnInit() {
    this.search();
  }

  search() {
    const fString$: Observable<string> = this.filterInput.valueChanges.pipe(
      startWith(''),
      debounceTime(700)
    );
    this.customers$ = combineLatest([this.customerService.search(''), fString$]).pipe(
      map(([customers, fString]) => {
        return customers.filter(cust => {
          return (cust.firstName + ' ' + cust.lastName).indexOf(fString) >= 0;
        });
      })
    );
  }
 
  addCustomer(): void {
    const dialogRef = this.dialog.open(CustomerCreateDialogComponent, {
      width: '250px',
      data: null
    });
    dialogRef.afterClosed().subscribe((customer: Customer) => {
      this.customerService.save(customer).subscribe(result => {
        this.search();
      });
    });
  }
  viewDetail(row: { customerId: any; }) {
    this.router.navigate([`./customer/${row.customerId}`]);
  }

}
