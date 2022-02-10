import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { CheckoutComponent } from './checkout.component';
import { CheckoutSucceessComponent } from './checkout-succeess/checkout-succeess.component';
import { FailPayComponent } from './fail-pay/fail-pay.component';

const routes: Routes  = [

  {path: '', component: CheckoutComponent},
  {path: 'result', children:[
    
    {path:'', component:CheckoutSucceessComponent,data:{breadcrumb: 'Trwa przetwarzanie płatności'} },
    { path: '/:error',redirectTo:'fail', pathMatch:'full' },
    { path: 'fail', component: FailPayComponent, data:{breadcrumb: 'Błąd transakcji'} },
   
  ]}
 
]

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
  
})
export class CheckoutRoutingModule { }
