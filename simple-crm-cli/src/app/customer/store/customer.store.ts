// actions using action creator, plus reducers that implement those actions.
import { createAction, props, createReducer, on, Action } from '@ngrx/store';
import { CustomerSearchCritiera, initialCustomerState, customerStateAdapter, CustomerState } from './customer.store.model';
import { Customer } from '../customer.model';

export const customerSearchAction = createAction(
  '[CUSTOMERS] Search Start',
  props<{criteria: CustomerSearchCritiera}>()
);

export const customersSearchCompleteAction = createAction(
  '[CUSTOMERS] Search Complete',
  props<{result: Customer[]}>()
);

// TODO: bonus exercise: add actions for customerSave and customerSaveComplete
//  and the matching reducer statements

export const customerFeatureKey = 'customer';

const rawCustomerReducer = createReducer(
  initialCustomerState,
  on(customerSearchAction, (state, action) => ({
    ...state,
    criteria: action.criteria,
    searchStatus: 'searching'
  })),
  on(customersSearchCompleteAction, (state, action) => {
    const clearedState = {
      ...customerStateAdapter.removeAll(state),
      searchStatus: 'complete'
    };
    return customerStateAdapter.addAll(action.result, clearedState);
  })
);

/** Provide reducer in AOT-compilation happy way */
export function customerReducer(state: CustomerState, action: Action) {
  return rawCustomerReducer(state, action);
}
