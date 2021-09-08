import { Injectable } from "@angular/core";

import { Actions, createEffect, ofType } from "@ngrx/effects";
import { of } from "rxjs";
import { switchMap, map, catchError } from "rxjs/operators";

import { InvoiceService } from "src/app/home/invoices/invoices.service";
import { CreateInvoice } from "src/app/add-invoice/create-invoice.model";
import * as AddInvoiceActions from './actions';

@Injectable()
export class AddInvoiceEffects {
  constructor(
    private actions$: Actions,
    private invoiceService: InvoiceService
  ) { }

  sendInvoice$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AddInvoiceActions.sendInvoice),
      switchMap((payLoad: CreateInvoice) =>
        this.invoiceService.sendInvoice(payLoad).pipe(
          map(() =>
            AddInvoiceActions.sendInvoiceSuccess()
          ),
          catchError((error) =>
            of(AddInvoiceActions.sendInvoiceError())
          )
        )))
  );
}
