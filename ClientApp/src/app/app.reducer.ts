import { InjectionToken } from "@angular/core";

import * as store from '@ngrx/store';
import * as fromRouter from '@ngrx/router-store';

import * as fromInvoices from './home/invoices/state/reducer'

export interface State {
  [fromInvoices.invoicesFeatureKey]: fromInvoices.State;
  router: fromRouter.RouterReducerState<any>;
}

export const ROOT_REDUCERS = new InjectionToken<
  store.ActionReducerMap<State, store.Action>
>('Root reducers token', {
  factory: () => ({
    [fromInvoices.invoicesFeatureKey]: fromInvoices.reducer,
    router: fromRouter.routerReducer,
  }),
});

export function logger(reducer: store.ActionReducer<State>): store.ActionReducer<State> {
  return (state, action) => {
    const result = reducer(state, action);
    console.groupCollapsed(action.type);
    console.log('prev state', state);
    console.log('action', action);
    console.log('next state', result);
    console.groupEnd();

    return result;
  };
};

export const metaReducers: store.MetaReducer<State>[] = [logger];
