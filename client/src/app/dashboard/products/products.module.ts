import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AddProductComponent } from './add-product/add-product.component';
import { ProductsComponent } from './products.component';
import { PaginationComponent } from 'ngx-bootstrap/pagination';


@NgModule({
  declarations: [
    AddProductComponent,
    ProductsComponent,
    PaginationComponent
  ],
  imports: [
    CommonModule
  ]
})
export class ProductsModule { }
