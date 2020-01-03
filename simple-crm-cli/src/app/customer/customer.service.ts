import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Customer } from './customer.model';

@Injectable()
export class CustomerService {

  constructor(public http: HttpClient) { }

  get(customerId: number) {
    return this.http.get<Customer>('/api/customers/' + customerId);
  }

  search(term: string): Observable<Customer[]> {
    return this.http.get<Customer[]>('/api/customers?term=' + term);
  }

  save(customer: Customer): Observable<Customer> {
    if (customer.customerId > 0) {
      const params = new HttpParams();
      params.set('id', '' + customer.customerId);
      return this.http.post<Customer>('/api/customers/:id', customer, {
        params // same as 'params: params'
      } );
    }
    return this.http.post<Customer>('/api/customers', customer);
  }
}
