import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AddInvoiceComponent } from './add-invoice/add-invoice.component';
import { HomeComponent } from './home/home.component';

const routes: Routes = [
  {
    path: '', component: HomeComponent,
  },
  {
    path: 'add', component: AddInvoiceComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
