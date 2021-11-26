import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routerProducts: Routes = [
];

@NgModule({
  imports: [RouterModule.forChild(routerProducts)],
  exports: [RouterModule]
})
export class ProductsRoutingModule { }
