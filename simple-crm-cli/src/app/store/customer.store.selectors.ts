
import { createFeatureSelector, createSelector } from '@ngrx/store';
import { customerFeatureKey } from './customer.store';
import { customerStateAdapter } from './customer.store.model';

// ngrx selectors to subscribe to specific portions of the customer feature state.

const getCustomerFeature = createFeatureSelector(customerFeatureKey);

const {
  selectAll: customerSearchResults
  // TODO: bonus exercise: add selector to load a single from the list by id
} = customerStateAdapter.getSelectors();

export const selectCustomers = createSelector(getCustomerFeature, customerSearchResults);
