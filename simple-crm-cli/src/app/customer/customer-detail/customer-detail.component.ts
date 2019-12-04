import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Customer } from '../customer.model';
import { CustomerService } from '../customer.service';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { MatSnackBar } from '@angular/material';

@Component({
  selector: 'crm-customer-detail',
  templateUrl: './customer-detail.component.html',
  styleUrls: ['./customer-detail.component.scss']
})
export class CustomerDetailComponent implements OnInit {

  customerId: number;
  customer: Customer;
  detailForm: FormGroup;

  constructor(private fb: FormBuilder,
              private customerService: CustomerService,
              private route: ActivatedRoute,
              private snackBar: MatSnackBar) {
                this.createForm();
   }

  ngOnInit() {
    this.customerId = +this.route.snapshot.params.id;
    this.customerService.get(this.customerId)
      .subscribe(cust => {
        this.customer = cust;
        this.detailForm.patchValue(cust);
      });
  }

  createForm() {
    this.detailForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      phoneNumber: [''],
      emailAddress: ['', [Validators.required, Validators.email]],
      preferredContactMethod: ['email'],
      statusCode: ['Prospect']
    });
  }

  save() {
    if (!this.detailForm.valid) { return; }
    const customer = { ...this.customer, ...this.detailForm.value };
    this.customerService.save(customer)
      .subscribe(result => {
        if (!result) {
          this.snackBar.open('Error saving customer record.', 'OOPS');
          return;
        }
        this.snackBar.open('Customer saved', 'OK');
      });
  }
}
