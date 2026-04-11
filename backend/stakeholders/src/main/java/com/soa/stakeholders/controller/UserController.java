package com.soa.stakeholders.controller;

import com.soa.stakeholders.dto.UserDTO;
import com.soa.stakeholders.exceptions.ResourceNotFoundException;
import com.soa.stakeholders.model.User;
import com.soa.stakeholders.service.UserService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.MediaType;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.*;

import java.security.Principal;
import java.util.List;
import java.util.stream.Collectors;

@RestController
@RequestMapping(value = "/api/users", produces = MediaType.APPLICATION_JSON_VALUE)
public class UserController {
    @Autowired
    private UserService userService;

    @GetMapping("/whoami")
    @PreAuthorize("hasAnyRole('TOURIST', 'GUIDE', 'ADMIN')")
    public User user(Principal user) {
        return this.userService.findByUsername(user.getName());
    }

    @GetMapping
    @PreAuthorize("hasRole('ADMIN')")
    public List<UserDTO> getAllUsers() {
        return userService.findAll().stream()
                .map(UserDTO::new)
                .collect(Collectors.toList());
    }

    @GetMapping("/{id}")
    @PreAuthorize("hasRole('ADMIN')")
    public UserDTO getUserById(@PathVariable Long id) {
        User user = userService.findById(id);

        if (user == null) {
            throw new ResourceNotFoundException("User not found with id: " + id);
        }

        return new UserDTO(user);
    }

    @PutMapping("/{id}/block")
    @PreAuthorize("hasRole('ADMIN')")
    public void toggleBlock(@PathVariable Long id, @RequestParam boolean block) {
        User user = userService.findById(id);
        user.setBlocked(block);
        userService.save(user);
    }

    @PutMapping("/profile")
    @PreAuthorize("hasAnyRole('TOURIST', 'GUIDE')")
    public UserDTO updateMyProfile(@RequestBody UserDTO userDto, Principal principal) {
        User currentUser = userService.findByUsername(principal.getName());
        User updatedUser = userService.update(currentUser.getId(), userDto);
        return new UserDTO(updatedUser);
    }
}
