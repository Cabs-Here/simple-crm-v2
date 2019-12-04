import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Customer } from '../customer.model';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'crm-customer-create-dialog',
  templateUrl: './customer-create-dialog.component.html',
  styleUrls: ['./customer-create-dialog.component.scss']
})
export class CustomerCreateDialogComponent implements OnInit {
  detailForm: FormGroup;

  constructor(private fb: FormBuilder,
              public dialogRef: MatDialogRef<CustomerCreateDialogComponent>,
              @Inject(MAT_DIALOG_DATA) public data: Customer | null) {
                this.detailForm = this.fb.group({
                  firstName: ['', Validators.required],
                  lastName: ['', Validators.required],
                  phoneNumber: [''],
                  emailAddress: ['', [Validators.required, Validators.email]],
                  preferredContactMethod: ['email'],
                  statusCode: ['Prospect']
                });
                if (this.data) {
                  this.detailForm.patchValue(this.data);
                }
               }

  ngOnInit() {
  }

  cancel() {
    this.dialogRef.close();
  }

  save() {
    if (!this.detailForm.valid) {
      return;
    }
    const customer = {...this.data, ...this.detailForm.value};
    this.dialogRef.close(customer);
  }

}
