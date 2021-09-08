import { Component, OnInit } from '@angular/core';

import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';

import { Invoice } from 'src/app/home/invoices/invoice.model';
import * as InvoiceActions from './state/actions';
import * as fromInvoices from './state/reducer';

@Component({
  selector: 'app-invoices',
  templateUrl: './invoices.component.html',
  styleUrls: ['./invoices.component.scss']
})
export class InvoicesComponent implements OnInit {

  invoiceList: Observable<Invoice[]>;
  isLoading: Observable<boolean>;
  
  constructor(private store: Store) { }

  ngOnInit(): void {
    this.invoiceList = this.store.select(fromInvoices.selectInvoiceList);
    this.isLoading = this.store.select(fromInvoices.selectIsLoading);
    this.store.dispatch(InvoiceActions.loadInvoices());
  }
}
