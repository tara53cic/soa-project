package com.soa.stakeholders.controller;

import com.soa.stakeholders.model.User;
import com.soa.stakeholders.service.UserService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.MediaType;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.security.Principal;

@RestController
@RequestMapping(value = "/auth", produces = MediaType.APPLICATION_JSON_VALUE)
public class UserController {
    @Autowired
    private UserService userService;

    @GetMapping("/whoami")
    @PreAuthorize("hasAnyRole('TOURIST', 'GUIDE')")
    public User user(Principal user) {
        return this.userService.findByUsername(user.getName());
    }
}
