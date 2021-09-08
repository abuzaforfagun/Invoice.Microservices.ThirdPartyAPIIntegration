import { createAction, props } from '@ngrx/store';

import { Invoice } from '../invoice.model'

export const loadInvoices = createAction('[invoices] LOAD_INVOICES');
export const loadInvoicesSuccess = createAction(
    '[invoices] LOAD_INVOICES_SUCCESS',
    props<{ payload: Invoice[] }>()
);
export const loadInvoicesFailure = createAction('[invoices] LOAD_INVOICES_FAILURE');
