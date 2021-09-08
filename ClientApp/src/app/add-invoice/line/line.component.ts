import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';

import { Line } from 'src/app/add-invoice/create-invoice.model';
import * as fromAddInvoice from '../state/reducer';
import * as AddInvoiceActions from '../state/actions';

@Component({
  selector: 'app-line',
  templateUrl: './line.component.html',
  styleUrls: ['./line.component.scss']
})
export class LineComponent implements OnInit {

  @Input() lineForm: FormGroup;
  lines: Observable<Line[]>;

  constructor(
    private fb: FormBuilder,
    private store: Store
  ) { }

  ngOnInit(): void {
    this.lines = this.store.select(fromAddInvoice.selectLineList);
  }

  onAddLineClick(): void {
    const line = this.lineForm.getRawValue();
    if(this.lineForm.valid) {
      this.store.dispatch(AddInvoiceActions.addLine(line));
      this.lineForm.reset();
    }
    
  }

}
