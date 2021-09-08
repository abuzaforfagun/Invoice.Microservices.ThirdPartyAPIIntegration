import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';

import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';

import { CreateInvoice, Line } from './create-invoice.model';
import * as fromInvoice from './state/reducer';
import * as AddInvoiceActions from './state/actions';

@Component({
  selector: 'app-add-invoice',
  templateUrl: './add-invoice.component.html',
  styleUrls: ['./add-invoice.component.scss']
})
export class AddInvoiceComponent implements OnInit {

  invoiceForm: FormGroup;
  debtorForm: FormGroup;
  lineForm: FormGroup;
  isLoading: Observable<boolean>;
  isAdded: Observable<boolean>;
  private lines: Line[];
  
  constructor(
    private fb: FormBuilder,
    private store: Store,
    private snackBar: MatSnackBar,
    private router: Router
    ) { }

  ngOnInit(): void {
    this.store.dispatch(AddInvoiceActions.initForm());
    this.isLoading = this.store.select(fromInvoice.selectIsLoading);
    this.invoiceForm = this.createInvoiceForm();
    this.lineForm = this.createLineForm();
    this.debtorForm = this.createDebtorForm();

    this.store.select(fromInvoice.selectLineList)
      .subscribe(lines => {
        this.lines = lines;
      });
    this.store.select(fromInvoice.selectIsAdded)
      .subscribe(isAdded => {
        if(isAdded && this.debtorForm.touched) {
          this.store.dispatch(AddInvoiceActions.initForm());
          this.router.navigate(['/']);
          this.snackBar.open(
            'Your request sent! You will be notified soon', '', {
              duration: 5000
            });
        }
      });
  }

  onSaveClick(): void {
    this.invoiceForm.markAllAsTouched();
    this.debtorForm.markAllAsTouched();

    if (this.lines.length === 0) {
      this.lineForm.markAllAsTouched();
    }
    
    if(this.lines.length === 0 || !this.invoiceForm.valid || !this.debtorForm.valid) {
      this.snackBar.open(
        'Please varify your input first!', '', {
          duration: 1500
        });
      return;
    }

    var datePipe=new DatePipe("en-US");
    let payload: CreateInvoice = this.invoiceForm.getRawValue();
    payload.debtor = this.debtorForm.getRawValue();
    payload.lines = this.lines;
    payload.date = datePipe.transform(payload.date,'yyyy-MM-dd');
    payload.dueDate = datePipe.transform(payload.dueDate,'yyyy-MM-dd');
    
    this.store.dispatch(AddInvoiceActions.sendInvoice(payload));
  }

  private createDebtorForm(): FormGroup {
    return this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', Validators.required],
      phone: ['', Validators.required],
      debtorType: [1, Validators.required],
      address: ['', [Validators.required]],
      zipCode: ['', Validators.required],
      city: ['', Validators.required],
    });
  }

  private createInvoiceForm(): FormGroup {
    return this.fb.group({
      date: [new Date(), Validators.required],
      dueDate: [new Date(), Validators.required],
      currency: ['DKK', Validators.required],
      campaignInitialRequest: [6, Validators.required]
    })
  }

  private createLineForm(): FormGroup {
    return this.fb.group({
      unitNetPrice: ['', Validators.required],
      description: ['', Validators.required],
      vatRate: ['', Validators.required],
      discountType: [0, Validators.required],
      discountValue: ['', Validators.required],
    })
  }
}
