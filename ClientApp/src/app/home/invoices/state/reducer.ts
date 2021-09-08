import { createFeatureSelector, createReducer, createSelector, on } from '@ngrx/store';

import { Invoice } from 'src/app/home/invoices/invoice.model';
import * as InvoiceActions from './actions';

export const invoicesFeatureKey = 'invoices';

export interface State {
  isLoading: boolean;
  invoiceList: Invoice[];
}

const initialState: State = {
  isLoading: false,
  invoiceList: []
};

export const reducer = createReducer(
  initialState,
  on(InvoiceActions.loadInvoices, (state) => ({
    ...state, isLoading: true,
  })),
  on(InvoiceActions.loadInvoicesSuccess, (state, { payload }) => ({
    ...state, isLoading: false, invoiceList: payload
  }))
);

export const selectInvoicseState = createFeatureSelector<State>(
  invoicesFeatureKey
);

export const selectInvoiceList = createSelector(
  selectInvoicseState,
  (state) => state.invoiceList
);

export const selectIsLoading = createSelector(
  selectInvoicseState,
  (state) => state.isLoading
);
