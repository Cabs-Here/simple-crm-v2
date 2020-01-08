import { Component, OnInit } from '@angular/core';
import { Customer } from '../customer.model';
import { MatTableDataSource } from '@angular/material';
import { CustomerService } from '../customer.service';
import { Observable } from 'rxjs';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { CustomerCreateDialogComponent } from '../customer-create-dialog/customer-create-dialog.component';
import { Router } from '@angular/router';

@Component({
  selector: 'crm-customer-list-page',
  templateUrl: './customer-list-page.component.html',
  styleUrls: ['./customer-list-page.component.scss']
})
export class CustomerListPageComponent implements OnInit {
  customers$: Observable<Customer[]>;
  dataSource: MatTableDataSource<Customer>;
  displayColumns = ['statusIcon', 'name', 'phone', 'email', 'lastContactDate', 'status', 'detail'];


  constructor(private customerService: CustomerService,
              public dialog: MatDialog,
              private router: Router) { }

  ngOnInit() {
    this.customers$ = this.customerService.search('');
  }
  addCustomer(): void {
    const dialogRef = this.dialog.open(CustomerCreateDialogComponent, {
      width: '250px',
      data: null
    });
    dialogRef.afterClosed().subscribe((customer: Customer) => {
      this.customerService.save(customer).subscribe(result => {
        this.customers$ = this.customerService.search('');
      });
    });
  }
  viewDetail(row: { customerId: any; }) {
    this.router.navigate([`./customer/${row.customerId}`]);
  }

}
