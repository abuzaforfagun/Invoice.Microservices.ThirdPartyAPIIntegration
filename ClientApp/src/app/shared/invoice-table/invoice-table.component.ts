import { Component, Input } from '@angular/core';

import { Observable } from 'rxjs';

@Component({
  selector: 'app-invoice-table',
  templateUrl: './invoice-table.component.html',
  styleUrls: ['./invoice-table.component.scss']
})
export class InvoiceTableComponent {

  @Input() invoices?: Observable<any>;
  tableColumns: string[] = [
    "referenceId",
    "creditorReference",
    "currency",
    "net",
    "gross",
    "remainder",
    "vat",
    "expireDate",
    "dueDate",
    "issueDate"
  ]

  constructor() { }
}
