import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { IUser } from 'src/app/shared/models/user';

@Injectable({
  providedIn: 'root'
})
export class AuthRoleGuard implements CanActivate {

  constructor(private accountService:AccountService, private router: Router, private toastr:ToastrService){}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) : Observable<boolean>{
    return this.accountService.getInfoAboutUser().pipe(map((res:IUser)=>{
      if(res==null){
        this.router.navigate(['account/signInAdmin'], {queryParams:{returnUrl:state.url}})
        return false;
      }else if(res.role=="ADMIN") return true;
      else{
      this.router.navigate(['/shop'], {queryParams:{returnUrl:state.url}})
      this.toastr.error("Brak uprawnnień administratora");
      return false;
      }
    }))

  //   return this.accountService.isAdmin().pipe(map((response:boolean) => {
  //     if (response) {
  //         return true;
  //     }
  //     this.router.navigate(['account/signInAdmin'], {queryParams:{returnUrl:state.url}})
  //     this.accountService.logout();
  //     this.toastr.error("Brak uprawnnień administratora")
  //     return false;
  // }), catchError((error) => {
  //     this.router.navigate(['account/signInAdmin']);
  //     return of(false);
  // }));
}
}
