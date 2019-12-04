import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Customer } from './customer.model';

@Injectable()
export class CustomerService {

  constructor(public http: HttpClient) { }

  get(customerId: number) {
    return this.http.get<Customer>('/api/customer/' + customerId);
  }

  search(term: string): Observable<Customer[]> {
    return this.http.get<Customer[]>('/api/customer/search?term=' + term);
  }

  save(customer: Customer): Observable<Customer> {
    return this.http.post<Customer>('/api/customer/save', customer);
  }
}
