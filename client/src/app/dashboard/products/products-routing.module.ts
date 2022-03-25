import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AddProductComponent } from './add-product/add-product.component';
import { ProductsComponent } from './products.component';

const routes: Routes = [
  {path:'admin/products',component:ProductsComponent, data:{breadcrumb: 'Produkt'}},
  {path:'admin/products/add', component: AddProductComponent,  data:{breadcrumb: 'Dodaj produkt'}}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProductsRoutingModule { }
