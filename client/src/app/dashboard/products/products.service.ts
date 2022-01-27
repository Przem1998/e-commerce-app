import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IProduct } from 'src/app/shared/models/product';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ProductsService {
baseUrl= environment.apiUrl;
  constructor(private http: HttpClient) { }
  getProducts(){
    return this.http.get(this.baseUrl+'products');
  }
}
