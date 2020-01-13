// The interface types contained in the store, and the top level state structure and initial state value for this module.
import { Customer } from '../customer.model';
import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { EntityAdapter } from '@ngrx/entity/src/models';

export interface CustomerSearchCritiera {
  term: string;
  // TODO: also add server side paging and sorting here.
}

export type SearchStatus = '' | 'searching' | 'complete'; // empty means not started.

export interface CustomerState extends EntityState<Customer> {
  searchStatus: SearchStatus;
  criteria: CustomerSearchCritiera;
}

export const customerStateAdapter: EntityAdapter<Customer> = createEntityAdapter<Customer>({
  selectId: (item: Customer) => item.customerId
});

export const initialCustomerState: CustomerState = customerStateAdapter.getInitialState({
  searchStatus: '',
  criteria: {term: ''}
});
