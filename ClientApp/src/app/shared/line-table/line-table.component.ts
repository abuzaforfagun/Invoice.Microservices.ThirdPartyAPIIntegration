import { Component, Input } from '@angular/core';

import { Observable } from 'rxjs';

@Component({
  selector: 'app-line-table',
  templateUrl: './line-table.component.html',
  styleUrls: ['./line-table.component.scss']
})
export class LineTableComponent {

  @Input() lines?: Observable<any>;
  tableColumns: string[] = [
    "unitNetPrice",
    "description",
    "vatRate",
    "discountValue"
  ]
}
