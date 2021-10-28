import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { CheckoutComponent } from './checkout.component';
import { CheckoutSucceessComponent } from './checkout-succeess/checkout-succeess.component';

const routes: Routes  = [
  {path: '', component: CheckoutComponent},
  {path: 'success', component: CheckoutSucceessComponent}
]

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CheckoutRoutingModule { }
