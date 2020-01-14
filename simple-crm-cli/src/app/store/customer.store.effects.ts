import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { customerSearchAction, customersSearchCompleteAction } from './customer.store';
import { catchError, map, switchMap } from 'rxjs/operators';
import { CustomerService } from '../customer.service';
import { EMPTY } from 'rxjs';
// ngrx effects that trigger side effects for specific actions.

@Injectable()
export class CustomerStoreEffects {
  constructor(
    private actions$: Actions,
    private custSvc: CustomerService
  ) { }

  searchCustomers$ = createEffect(() => this.actions$.pipe(
    ofType(customerSearchAction),
    switchMap(({criteria}) =>
      this.custSvc.search(criteria.term).pipe(
        map(
          data => customersSearchCompleteAction({result: data})
        ),
        catchError(err => {
          console.error(err);
          return EMPTY;
        })
      )
    )
  ));

  // TODO: bonus exercise: add effect to handle action saveCustomer
}
