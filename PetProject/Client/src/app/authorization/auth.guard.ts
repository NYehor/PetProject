import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthorizeService } from './auth.service';
import { Observable } from 'rxjs';
import { tap, map } from 'rxjs/operators';
import { Router } from '@angular/router';
import { Injectable } from '@angular/core';

@Injectable()
export class AuthGuard implements CanActivate {

    constructor(private router: Router, private authService: AuthorizeService) {}

    canActivate(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): Observable<boolean> | boolean {

        return this.authService.isAuthenticated().pipe(
            tap(isLoggedIn => {
                if (isLoggedIn) {
                    this.router.navigate([]);
                }
            }),
            map(isLoggedIn => !isLoggedIn)
        );
    }

}