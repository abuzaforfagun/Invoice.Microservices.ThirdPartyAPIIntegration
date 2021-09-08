import { createFeatureSelector, createReducer, createSelector, on } from "@ngrx/store";

import { BasicData, Debtor, Line } from "src/app/add-invoice/create-invoice.model";
import * as AddInvoiceActions from './actions';

export const addInvoiceFeatureKey = 'addInvoice';

export interface State {
    isLoading: boolean;
    isAdded: boolean;
    basicInvoiceData: BasicData;
    debtor: Debtor;
    lines: Line[];
}

const initialState: State = {
    isLoading: false,
    isAdded: false,
    basicInvoiceData: {
        currency: 'DKK',
        date: new Date().toJSON(),
        dueDate: new Date().toJSON(),
        campaignInitialRequest: 6
    },
    debtor: {
        lastName: '',
        email: '',
        phone: '',
        debtorType: 0,
        address: '',
        zipCode: '',
        city: '',
    },
    lines: []
}

export const reducer = createReducer(
    initialState,
    on(AddInvoiceActions.initForm, (state) => ({
        ...state, initialState
    })),
    on(AddInvoiceActions.addLine, (state, payload) => ({
        ...state, lines: state?.lines.concat(payload)
    })),
    on(AddInvoiceActions.sendInvoice, (state, payload) => ({
        ...state, isLoading: true
    })),
    on(AddInvoiceActions.sendInvoiceSuccess, (state) => ({
        ...state, isLoading: false, isAdded: true
    })),
    on(AddInvoiceActions.sendInvoiceError, (state) => ({
        ...state, isLoading: false
    }))
);

export const selectAddInvoiceState = createFeatureSelector<State>(
    addInvoiceFeatureKey
);

export const selectLineList = createSelector(
    selectAddInvoiceState,
    (state) => state.lines
);

export const selectIsLoading = createSelector(
    selectAddInvoiceState,
    (state) => state.isLoading
);

export const selectIsAdded = createSelector(
    selectAddInvoiceState,
    (state) => state.isAdded
);
