import { Injectable } from '@angular/core';

import { Actions, createEffect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';

import { Invoice } from 'src/app/home/invoices/invoice.model';
import { InvoiceService } from '../invoices.service';
import * as InvoiceActions from './actions';

@Injectable()
export class InvoiceEffects {
    constructor(
        private actions$: Actions,
        private invoiceService: InvoiceService
    ) { }

    loadCollection$ = createEffect(() =>
        this.actions$.pipe(
            ofType(InvoiceActions.loadInvoices),
            switchMap(() =>{
                return this.invoiceService.loadInvoices().pipe(
                    map((payload: Invoice[]) =>
                        InvoiceActions.loadInvoicesSuccess({ payload })
                    ),
                    catchError((error) =>
                        of(InvoiceActions.loadInvoicesFailure())
                    )
                );
            })));
}
